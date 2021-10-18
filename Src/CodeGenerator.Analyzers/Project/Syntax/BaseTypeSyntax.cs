using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace CodeGenerator.Analyzers.Framework.Project.Syntax
{
    public class BaseTypeSyntax
    {
        private SimpleBaseTypeSyntax _syntax;
        private ITypeSymbol _typeSymbol;
        private INamedTypeSymbol _namedTypeSymbol;

        public BaseTypeSyntax(SimpleBaseTypeSyntax syntax, Compilation compilation)
        {
            _syntax = syntax;


            var semantic = compilation.GetSemanticModel(syntax.SyntaxTree);
            var _attributeSymbol = semantic.GetSymbolInfo(syntax.Type);

            _namedTypeSymbol = (_attributeSymbol.Symbol as INamedTypeSymbol);
            _typeSymbol = (_attributeSymbol.Symbol as ITypeSymbol);

            var arrays = syntax.DescendantNodes().OfType<TypeParameterSyntax>().ToArray();
        }

        public bool IsGenericType { get => _namedTypeSymbol.IsGenericType; }
        public string TypeName { get => _namedTypeSymbol.ConstructedFrom.ToDisplayString(); }
        public string[] TypeParam { get => _namedTypeSymbol.TypeArguments.Select(_symbol => _symbol.ToDisplayString()).ToArray(); }

        public bool IsClass => _typeSymbol.TypeKind == TypeKind.Class;
        public bool IsInterface => _typeSymbol.TypeKind == TypeKind.Interface;
    }
}
