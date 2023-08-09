using Luban.Defs;
using Luban.Python.TypeVisitors;
using Luban.Types;
using Luban.Utils;
using Scriban.Runtime;

namespace Luban.Python.TemplateExtensions;

public class PythonCommonTemplateExtension : ScriptObject
{

    public static string FullName(DefTypeBase type)
    {
        return TypeUtil.MakePyFullName(type.Namespace, type.Name);
    }
    
    public static string StrFullName(string fullName)
    {
        return fullName.Replace(".", "_");
    }
    
    public static string EscapeComment(string comment)
    {
        return System.Web.HttpUtility.HtmlEncode(comment).Replace("\n", "<br/>");
    }
    
    public static string Deserialize(string fieldName, string jsonVarName, TType type)
    {
        if (type.IsNullable)
        {
            return $"if {jsonVarName} != None: {type.Apply(PyUnderlyingDeserializeVisitor.Ins, jsonVarName, fieldName)}";
        }
        else
        {
            return type.Apply(PyUnderlyingDeserializeVisitor.Ins, jsonVarName, fieldName);
        }
    }

    public static string DeserializeField(string fieldName, string jsonVarName, string jsonFieldName, TType type)
    {
        if (type.IsNullable)
        {
            return $"if {jsonVarName}.get('{jsonFieldName}') != None: {type.Apply(PyUnderlyingDeserializeVisitor.Ins, $"{jsonVarName}['{jsonFieldName}']", fieldName)}";
        }
        else
        {
            return type.Apply(PyUnderlyingDeserializeVisitor.Ins, $"{jsonVarName}['{jsonFieldName}']", fieldName);
        }
    }
}