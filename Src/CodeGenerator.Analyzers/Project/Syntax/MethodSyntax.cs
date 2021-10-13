using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.Analyzers.Framework.Project.Syntax
{
    public class MethodSyntax
    {
        private MethodDeclarationSyntax _methodDeclarationSyntax;
        private ICollection<ParameterSyntax> _parameters;

        public MethodSyntax(MethodDeclarationSyntax methodDeclarationSyntax)
        {
            _methodDeclarationSyntax = methodDeclarationSyntax;
            _parameters = _methodDeclarationSyntax.ParameterList.Parameters.Select(_syntax => new ParameterSyntax(_syntax)).ToList();
        }

        public string Name { get => _methodDeclarationSyntax.Identifier.ValueText; }
        public string ReturnType { get => _methodDeclarationSyntax.ReturnType.ToFullString(); }
        public ICollection<ParameterSyntax> Parameters { get => _parameters; }
        public string Comment { get => _methodDeclarationSyntax.Comment(); }
        public bool IsPublic { get => _methodDeclarationSyntax.IsPublic(); }
        public bool IsStatic { get => _methodDeclarationSyntax.IsStatic(); }
        public bool IsInternal { get => _methodDeclarationSyntax.IsInternal(); }
        public bool IsPrivate { get => _methodDeclarationSyntax.IsPrivate(); }
        public bool IsProtected { get => _methodDeclarationSyntax.IsProtected(); }
        public bool IsAbstract { get => _methodDeclarationSyntax.IsAbstract(); }
        public bool IsVirtual { get => _methodDeclarationSyntax.IsVirtual(); }
    }
}
