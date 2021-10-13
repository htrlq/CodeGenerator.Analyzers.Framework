using CodeGenerator.Analyzers.Framework.Project;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Diagnostics;

namespace SampleAnalyzer
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SampleAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(
                    Descriptors.AnalyzerDescriptor,
                    Descriptors.CompileUnitDescriptor
                );
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            Debugger.Launch();

            //context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            //context.EnableConcurrentExecution();

            //context.RegisterSyntaxNodeAction(AnalyzerSyntaxNode, SyntaxKind.CompilationUnit);
            context.RegisterCompilationAction(CompilationAnalysisContext);
        }

        private static void CompilationAnalysisContext(CompilationAnalysisContext context)
        {
            Debugger.Log(0, "title", "message");
            if (ProjectContextFactory.TryGetContext(context, out ProjectContext projectContext))
            {
                if (projectContext.CompilationUnit.Count > 0)
                    ExecuteAnalyzer(projectContext);
                else
                    context.ReportDiagnostic(Diagnostic.Create(Descriptors.CompileUnitDescriptor, null));
            }
            else
            {
                context.ReportDiagnostic(Diagnostic.Create(Descriptors.AnalyzerDescriptor, null));
            }
        }

        private static void ExecuteAnalyzer(ProjectContext context)
        {
            foreach (var @unit in context.CompilationUnit)
            {
                foreach (var @class in unit?.Classes)
                {
                    if (@class.IsPublic)
                        Trace.WriteLine($"{@class.Name} Public");

                    if (@class.IsStatic)
                        Trace.WriteLine($"{@class.Name} Static");

                    if (@class.IsInternal)
                        Trace.WriteLine($"{@class.Name} Internal");

                    if (@class.IsSealed)
                        Trace.WriteLine($"{@class.Name} Sealed");

                    if (@class.IsPartial)
                        Trace.WriteLine($"{@class.Name} Partial");

                    if (@class.IsAbstract)
                        Trace.WriteLine($"{@class.Name} Abstract");

                    Trace.WriteLine(@class.Comment);

                    foreach (var @method in @class.Methods)
                    {
                        if (@method.IsPublic)
                            Trace.WriteLine($"{@method.Name} IsPublic");

                        if (@method.IsStatic)
                            Trace.WriteLine($"{@method.Name} Static");

                        if (@method.IsInternal)
                            Trace.WriteLine($"{@method.Name} Internal");

                        if (@method.IsPrivate)
                            Trace.WriteLine($"{@method.Name} Private");

                        if (@method.IsProtected)
                            Trace.WriteLine($"{@method.Name} Protected");

                        if (@method.IsAbstract)
                            Trace.WriteLine($"{@method.Name} Abstract");

                        if (@method.IsVirtual)
                            Trace.WriteLine($"{@method.Name} Virtual");

                        foreach (var @param in method.Parameters)
                        {
                            Trace.WriteLine($"{@param.Type} {@param.Name}");
                        }

                        Trace.WriteLine($"{method.ReturnType}");
                        Trace.WriteLine(method.Comment);
                    }
                }
            }
        }
    }
}
