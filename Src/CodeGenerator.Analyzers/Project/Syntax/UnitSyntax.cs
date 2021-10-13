using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Analyzers.Framework.Project.Syntax
{
    public class UnitSyntax
    {
        private CompilationUnitSyntax _compilationUnitSyntax;

        public UnitSyntax(CompilationUnitSyntax compilationUnitSyntax)
        {
            _compilationUnitSyntax = compilationUnitSyntax;

            if (_compilationUnitSyntax.DescendantNodes().OfType<NamespaceDeclarationSyntax>().Any())
            {
                var @namespace = _compilationUnitSyntax.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
                Namespace = @namespace.Name.ToFullString();

                var usings = new HashSet<string>();

                foreach (var @using in _compilationUnitSyntax.Usings)
                {
                    if (!usings.Contains(@using.ToFullString()))
                        usings.Add(@using.ToFullString());
                }

                Usings = usings;

                Classes = @namespace.DescendantNodes().OfType<ClassDeclarationSyntax>().Select(_syntax => new ClassSyntax(_syntax)).ToList();
            }
        }

        public string? Namespace { get; }
        public ICollection<string>? Usings { get; }
        public ICollection<ClassSyntax>? Classes { get; }
    }
}
