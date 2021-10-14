using CodeGenerator.Analyzers.Framework.Project.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using TAttributeListSyntax = CodeGenerator.Analyzers.Framework.Project.Syntax.AttributeListSyntax;

namespace CodeGenerator.Analyzers.Framework.Project
{
    public class ProjectContext
    {
        public List<TAttributeListSyntax> CompilationUnitAttributeList { get; }
        public List<UnitSyntax> CompilationUnit { get; }

        public ProjectContext(List<TAttributeListSyntax> attributeListSyntaxs, List<UnitSyntax> compilationUnit)
        {
            CompilationUnitAttributeList = attributeListSyntaxs;
            CompilationUnit = compilationUnit.RemoveNull();
        }
    }
}
