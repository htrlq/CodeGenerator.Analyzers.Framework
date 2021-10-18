using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.Analyzers.Framework.Project.Syntax
{
    public class ClassSyntax
    {
        private ClassDeclarationSyntax _classDeclarationSyntax;
        private ICollection<MethodSyntax> _methods;
        private ICollection<BaseTypeSyntax> _baseTypes;

        public ClassSyntax(ClassDeclarationSyntax classDeclarationSyntax, Compilation compilation)
        {
            _classDeclarationSyntax = classDeclarationSyntax;
            var methodSyntaxes = _classDeclarationSyntax.DescendantNodes().OfType<MethodDeclarationSyntax>();
            _methods = methodSyntaxes.Select(_syntax => new MethodSyntax(_syntax)).ToList();

            if (_classDeclarationSyntax.IsBaseTypes())
            {
                var baseTypeList = _classDeclarationSyntax.DescendantNodes().OfType<BaseListSyntax>().FirstOrDefault();

                _baseTypes = baseTypeList.Types.OfType<SimpleBaseTypeSyntax>().Select(_syntax => new BaseTypeSyntax(_syntax, compilation)).ToList();
            }
            else
            {
                _baseTypes = new List<BaseTypeSyntax>();
            }
        }

        public string Name { get => _classDeclarationSyntax.Identifier.ValueText; }
        public string Comment { get => _classDeclarationSyntax.Comment(); }
        public bool IsPublic { get => _classDeclarationSyntax.IsPublic(); }
        public bool IsStatic { get => _classDeclarationSyntax.IsStatic(); }
        public bool IsInternal { get => _classDeclarationSyntax.IsInternal(); }
        public bool IsSealed { get => _classDeclarationSyntax.IsSealed(); }
        public bool IsPartial { get => _classDeclarationSyntax.IsPartial(); }
        public bool IsAbstract { get => _classDeclarationSyntax.IsAbstract(); }

        public ICollection<MethodSyntax> Methods => _methods;
        public ICollection<BaseTypeSyntax> BaseTypes => _baseTypes;
    }
}
