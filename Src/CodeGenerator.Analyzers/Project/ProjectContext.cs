using CodeGenerator.Analyzers.Framework.Project.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.Analyzers.Framework.Project
{
    public class ProjectContext
    {
        public List<AttributeListSyntax> CompilationUnitAttributeList { get; }
        public List<UnitSyntax> CompilationUnit { get; }

        public ProjectContext(List<AttributeListSyntax> attributeListSyntaxs, List<UnitSyntax> compilationUnit)
        {
            CompilationUnitAttributeList = attributeListSyntaxs;
            CompilationUnit = compilationUnit.RemoveNull();
        }
    }
}
