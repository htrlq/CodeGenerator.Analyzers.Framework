using CodeGenerator.Analyzers.Framework.Project.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
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
            var classes = GetClassDeclarationSyntaxs(syntaxTrees);

            if (attributes.Any() && classes.Any())
            {
                projectContext = new ProjectContext(attributes.ToList(), classes.ToList());
                return true;
            }

            return false;
        }

        private static IEnumerable<AttributeListSyntax> GetAttributeLists(IEnumerable<SyntaxTree> syntaxTrees)
        {
            foreach(var tree in syntaxTrees)
            {
                if (tree.TryGetRoot(out SyntaxNode? node) && node is CompilationUnitSyntax compilationUnit)
                {
                    if (compilationUnit.DescendantNodes().OfType<AttributeListSyntax>().Any())
                        yield return node.DescendantNodes().OfType<AttributeListSyntax>().FirstOrDefault();
                }
            }
        }

        private static IEnumerable<UnitSyntax> GetClassDeclarationSyntaxs(IEnumerable<SyntaxTree> syntaxTrees)
        {
            foreach (var tree in syntaxTrees)
            {
                if (tree.TryGetRoot(out SyntaxNode? node) && node is CompilationUnitSyntax compilationUnit)
                {
                    yield return new UnitSyntax(compilationUnit);
                }
            }
        }
    }
}
