using System;
using System.Web.Script.Serialization;


namespace TalentsProfile.BAL
{
    /// <summary>
    /// This class deserializes JavaScript text value into JavaScript object type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Serializer<T>
    {
        public static T DeserializeText<T>(string text)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var obj = js.Deserialize<T>(text);
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}
