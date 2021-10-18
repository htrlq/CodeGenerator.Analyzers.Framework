using CodeGenerator.Analyzers.Framework.Project;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;

namespace EntityToCrudAnalyzer
{
    public class ConfigInstance
    {
        public bool IsBatch { get; set; }
        public string TargetDirectory { get; set; }
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EntityToCrudAnalyzerAnalyzer : DiagnosticAnalyzer
    {

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    Descriptors.ConfigDescriptor,
                    Descriptors.ConfigFailDescriptor,
                    Descriptors.AnalyzerDescriptor,
                    Descriptors.CompileUnitDescriptor
                );
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            //var s = Debugger.IsAttached;
            Debugger.Launch();
            Debug.WriteLine("Analyzer Initialize");
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterCompilationAction(CompilationAnalysisContext);
        }

        private static void CompilationAnalysisContext(CompilationAnalysisContext context)
        {
            //Debugger.Log(0, "title", "message");

            //foreach (var analyzerFile in context.Options.AdditionalFiles)
            //{
            //    var source = analyzerFile.GetText();
            //}
            Debug.WriteLine("Analyzer CompilationAnalysisContext");
            if (context.Options.AdditionalFiles.Any(_file=>_file.Path.Contains("Config.xmls")))
            {
                if (ProjectContextFactory.TryGetContext(context, out ProjectContext projectContext))
                {
                    var analyzerFile = context.Options.AdditionalFiles.FirstOrDefault(_file => _file.Path.Contains("Config.xmls"));
                    var source = analyzerFile.GetText();

                    try
                    {
                        var config = GetConfig(source);
                        if (projectContext.CompilationUnit.Count > 0)
                            ExecuteAnalyzer(projectContext, config);
                        else
                        {
                            Debug.WriteLine($"Analyzer CompilationAnalysisContext:{Descriptors.CompileUnitDescriptor.MessageFormat}");
                            context.ReportDiagnostic(Diagnostic.Create(Descriptors.CompileUnitDescriptor, null));
                        }
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine($"Analyzer CompilationAnalysisContext:{string.Format(Descriptors.AnalyzerDescriptor.MessageFormat.ToString(), ex.Message)}");
                        context.ReportDiagnostic(Diagnostic.Create(Descriptors.ConfigFailDescriptor, null, ex.Message));
                    }
                }
                else
                {
                    Debug.WriteLine($"Analyzer CompilationAnalysisContext:{Descriptors.AnalyzerDescriptor.MessageFormat}");
                    context.ReportDiagnostic(Diagnostic.Create(Descriptors.AnalyzerDescriptor, null));
                }
            }
            else
            {
                Debug.WriteLine($"Analyzer CompilationAnalysisContext:{Descriptors.ConfigDescriptor.MessageFormat}");
                context.ReportDiagnostic(Diagnostic.Create(Descriptors.ConfigDescriptor, null));
            }
        }

        private static ConfigInstance GetConfig(SourceText text)
        {
            var xmlSource = text.ToString();
            var xml = new XmlDocument();

            try
            {
                xml.LoadXml(xmlSource);
                var rootNode = xml.SelectSingleNode("Root");

                if (rootNode != null)
                {
                    var value = new ConfigInstance();

                    foreach (XmlNode xmlNode in rootNode.ChildNodes)
                    {
                        if (xmlNode.Name == "IsBatch")
                        {
                            var valueAttribute = xmlNode.Attributes["Value"];

                            if (valueAttribute != null)
                            {
                                if (bool.TryParse(valueAttribute.Value, out bool isBatch))
                                {
                                    value.IsBatch = isBatch;
                                }
                            }
                        }

                        if (xmlNode.Name == "TargetDirectory")
                        {
                            var valueAttribute = xmlNode.Attributes["Value"];

                            if (valueAttribute != null)
                            {
                                value.TargetDirectory = valueAttribute.Value;
                            }
                        }
                    }

                    return value;
                }

                throw new Exception($"未找到Root节点");
            }
            catch
            {
                throw;
            }
        }

        private static void ExecuteAnalyzer(ProjectContext context, ConfigInstance config)
        {
            foreach (var @unit in context.CompilationUnit)
            {
                foreach (var @class in unit?.Classes)
                {
                    if (@class.IsPublic && @class.BaseTypes.Any())
                    {
                        if (@class.BaseTypes.Any(_syntaxt=>_syntaxt.IsInterface && _syntaxt.TypeName == "Entity.Abstact.IEntity<Type>"))
                        {
                            var baseType = @class.BaseTypes.FirstOrDefault(_syntaxt => _syntaxt.IsInterface && _syntaxt.TypeName == "Entity.Abstact.IEntity<Type>");
                            var genericType = baseType.TypeParam.FirstOrDefault();

                            var usings = new UsingDirectiveSyntax[] {
                                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Collections.Generic")),
                                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Entity.Abstact")),
                                SyntaxFactory.UsingDirective(SyntaxFactory.ParseName($"{unit.Namespace}"))
                            };

                            var @newUnit = SyntaxFactory.CompilationUnit();
                            //@newUnit.AddUsings(usings);

                            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("SourceEntity.CodeGenerator")).NormalizeWhitespace();

                            var className = config.IsBatch ? $"{@class.Name}BatchRespositor" : $"{@class.Name}Respositor";
                            var classDeclaration = SyntaxFactory.ClassDeclaration(className);

                            classDeclaration = classDeclaration.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

                            if (config.IsBatch)
                            {
                                classDeclaration = classDeclaration.AddBaseListTypes(
                                    SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"IEntityBatchRespository<{@class.Name},{genericType}>"))
                                );
                            }
                            else
                            {
                                classDeclaration = classDeclaration.AddBaseListTypes(
                                    SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName($"IEntityRespository<{@class.Name},{genericType}>"))
                                );
                            }

                            var body = SyntaxFactory.ParseStatement("throw new NotImplementedException();");

                            var AddMethodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "Add")
                                .AddParameterListParameters(
                                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("entity")).WithType(SyntaxFactory.ParseTypeName($"{@class.Name}"))
                                )
                                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                                .WithBody(SyntaxFactory.Block(body));

                            var UpdateMethodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "Update")
                                .AddParameterListParameters(
                                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("entity")).WithType(SyntaxFactory.ParseTypeName($"{@class.Name}"))
                                )
                                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                                .WithBody(SyntaxFactory.Block(body));

                            var DeleteMethodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "Delete")
                                .AddParameterListParameters(
                                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("entity")).WithType(SyntaxFactory.ParseTypeName($"{@class.Name}"))
                                )
                                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                                .WithBody(SyntaxFactory.Block(body));

                            var GetByIdMethodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName($"{@class.Name}"), "GetById")
                            .AddParameterListParameters(
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("id")).WithType(SyntaxFactory.ParseTypeName($"{genericType}"))
                            )
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .WithBody(SyntaxFactory.Block(body));

                            if (config.IsBatch)
                            {
                                var AddRangeMethodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "AddRange")
                                .AddParameterListParameters(
                                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("entities")).WithType(SyntaxFactory.ParseTypeName($"IEnumerable<{@class.Name}>"))
                                )
                                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                                .WithBody(SyntaxFactory.Block(body));

                                var UpdateRangeMethodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "UpdateRange")
                                    .AddParameterListParameters(
                                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("entities")).WithType(SyntaxFactory.ParseTypeName($"IEnumerable<{@class.Name}>"))
                                    )
                                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                                    .WithBody(SyntaxFactory.Block(body));

                                var DeleteRangeMethodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "DeleteRange")
                                    .AddParameterListParameters(
                                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("entities")).WithType(SyntaxFactory.ParseTypeName($"IEnumerable<{@class.Name}>"))
                                    )
                                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                                    .WithBody(SyntaxFactory.Block(body));

                                classDeclaration = classDeclaration.AddMembers(AddMethodDeclaration, AddRangeMethodDeclaration, UpdateMethodDeclaration, UpdateRangeMethodDeclaration, DeleteMethodDeclaration, DeleteRangeMethodDeclaration, GetByIdMethodDeclaration);
                            }
                            else
                            {
                                classDeclaration = classDeclaration.AddMembers(AddMethodDeclaration, UpdateMethodDeclaration, DeleteMethodDeclaration, GetByIdMethodDeclaration);
                            }


                            @namespace  = @namespace.AddMembers(classDeclaration);
                            newUnit = newUnit.AddUsings(usings);
                            newUnit = newUnit.AddMembers(@namespace);

                            var code = newUnit
                                .NormalizeWhitespace()
                                .ToFullString();

                            var sourceGenerator = config.TargetDirectory;
                            File.WriteAllText(Path.Combine(sourceGenerator, $"{className}.cs"), code);
                        }
                    }
                }
            }
        }
    }
}
