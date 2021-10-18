using CodeGenerator.Analyzers.Framework.Project.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using TAttributeListSyntax = CodeGenerator.Analyzers.Framework.Project.Syntax.AttributeListSyntax;
using SAttributeListSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax;
using System.Linq;

namespace CodeGenerator.Analyzers.Framework.Project
{
    public static class ProjectContextFactory
    {
        public static bool TryGetContext(CompilationAnalysisContext context, out ProjectContext projectContext)
        {
            projectContext = null;

            var syntaxTrees = context.Compilation.SyntaxTrees;

            var attributes = GetAttributeLists(syntaxTrees);
            var tAttributes = attributes.Select(_syntax => new TAttributeListSyntax(_syntax, context.Compilation)).ToList();

            var classes = GetClassDeclarationSyntaxs(syntaxTrees, context.Compilation);

            if (attributes.Any() && classes.Any())
            {
                projectContext = new ProjectContext(tAttributes, classes.ToList());
                return true;
            }

            return false;
        }

        private static IEnumerable<SAttributeListSyntax> GetAttributeLists(IEnumerable<SyntaxTree> syntaxTrees)
        {
            foreach(var tree in syntaxTrees)
            {
                if (tree.TryGetRoot(out SyntaxNode? node) && node is CompilationUnitSyntax compilationUnit)
                {
                    if (compilationUnit.DescendantNodes().OfType<SAttributeListSyntax>().Any())
                        yield return node.DescendantNodes().OfType<SAttributeListSyntax>().FirstOrDefault();
                }
            }
        }

        private static IEnumerable<UnitSyntax> GetClassDeclarationSyntaxs(IEnumerable<SyntaxTree> syntaxTrees, Compilation compilation)
        {
            foreach (var tree in syntaxTrees)
            {
                if (tree.TryGetRoot(out SyntaxNode? node) && node is CompilationUnitSyntax compilationUnit)
                {
                    yield return new UnitSyntax(compilationUnit, compilation);
                }
            }
        }
    }
}
