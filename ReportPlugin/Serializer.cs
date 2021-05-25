using System;
using System.Xml.Serialization;	 // For serialization of an object to an XML Document file.
using System.Runtime.Serialization.Formatters.Binary; // For serialization of an object to an XML Binary file.
using System.IO;				 // For reading/writing data to an XML file.
using System.IO.IsolatedStorage; // For accessing user isolated data.

namespace IM.ReportPlugin
{

  /// <summary>
  /// Facade to XML serialization and deserialization of strongly typed objects to/from an XML file.
  /// 
  /// References: XML Serialization at http://samples.gotdotnet.com/:
  /// http://samples.gotdotnet.com/QuickStart/howto/default.aspx?url=/quickstart/howto/doc/xmlserialization/rwobjfromxml.aspx
  /// </summary>
  public static class Serializer<T> where T : class // Specify that T must be a class.
  {
    #region Load methods

    /// <summary>
    /// Loads an object from an XML file in Document format.
    /// </summary>
    /// <example>
    /// <code>
    /// serializableObject = ObjectXMLSerializer&lt;SerializableObject&gt;.Load(@"C:\XMLObjects.xml");
    /// </code>
    /// </example>
    /// <param name="path">Path of the file to load the object from.</param>
    /// <returns>Object loaded from an XML file in Document format.</returns>
    public static T Load(string path)
    {
      T serializableObject = LoadFromDocumentFormat(null, path, null);
      return serializableObject;
    }


    /// <summary>
    /// Loads an object from an XML file in Document format, supplying extra data types to enable deserialization of custom types within the object.
    /// </summary>
    /// <example>
    /// <code>
    /// serializableObject = ObjectXMLSerializer&lt;SerializableObject&gt;.Load(@"C:\XMLObjects.xml", new Type[] { typeof(MyCustomType) });
    /// </code>
    /// </example>
    /// <param name="path">Path of the file to load the object from.</param>
    /// <param name="extraTypes">Extra data types to enable deserialization of custom types within the object.</param>
    /// <returns>Object loaded from an XML file in Document format.</returns>
    public static T Load(string path, System.Type[] extraTypes)
    {
      T serializableObject = LoadFromDocumentFormat(extraTypes, path, null);
      return serializableObject;
    }

    /// <summary>
    /// Loads an object from an XML file in Document format, located in a specified isolated storage area.
    /// </summary>
    /// <example>
    /// <code>
    /// serializableObject = ObjectXMLSerializer&lt;SerializableObject&gt;.Load("XMLObjects.xml", IsolatedStorageFile.GetUserStoreForAssembly());
    /// </code>
    /// </example>
    /// <param name="fileName">Name of the file in the isolated storage area to load the object from.</param>
    /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to load the object from.</param>
    /// <returns>Object loaded from an XML file in Document format located in a specified isolated storage area.</returns>
    public static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory)
    {
      T serializableObject = LoadFromDocumentFormat(null, fileName, isolatedStorageDirectory);
      return serializableObject;
    }


    #endregion

    #region Save methods

    /// <summary>
    /// Saves an object to an XML file in Document format.
    /// </summary>
    /// <example>
    /// <code>        
    /// SerializableObject serializableObject = new SerializableObject();
    /// 
    /// ObjectXMLSerializer&lt;SerializableObject&gt;.Save(serializableObject, @"C:\XMLObjects.xml");
    /// </code>
    /// </example>
    /// <param name="serializableObject">Serializable object to be saved to file.</param>
    /// <param name="path">Path of the file to save the object to.</param>
    public static void Save(T serializableObject, string path)
    {
      SaveToDocumentFormat(serializableObject, null, path, null);
    }


    /// <summary>
    /// Saves an object to an XML file in Document format, supplying extra data types to enable serialization of custom types within the object.
    /// </summary>
    /// <example>
    /// <code>        
    /// SerializableObject serializableObject = new SerializableObject();
    /// 
    /// ObjectXMLSerializer&lt;SerializableObject&gt;.Save(serializableObject, @"C:\XMLObjects.xml", new Type[] { typeof(MyCustomType) });
    /// </code>
    /// </example>
    /// <param name="serializableObject">Serializable object to be saved to file.</param>
    /// <param name="path">Path of the file to save the object to.</param>
    /// <param name="extraTypes">Extra data types to enable serialization of custom types within the object.</param>
    public static void Save(T serializableObject, string path, System.Type[] extraTypes)
    {
      SaveToDocumentFormat(serializableObject, extraTypes, path, null);
    }

    /// <summary>
    /// Saves an object to an XML file in Document format, located in a specified isolated storage area.
    /// </summary>
    /// <example>
    /// <code>        
    /// SerializableObject serializableObject = new SerializableObject();
    /// 
    /// ObjectXMLSerializer&lt;SerializableObject&gt;.Save(serializableObject, "XMLObjects.xml", IsolatedStorageFile.GetUserStoreForAssembly());
    /// </code>
    /// </example>
    /// <param name="serializableObject">Serializable object to be saved to file.</param>
    /// <param name="fileName">Name of the file in the isolated storage area to save the object to.</param>
    /// <param name="isolatedStorageDirectory">Isolated storage area directory containing the XML file to save the object to.</param>
    public static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory)
    {
      SaveToDocumentFormat(serializableObject, null, fileName, isolatedStorageDirectory);
    }

        #endregion
        #region Private

        private static T LoadFromDocumentFormat(System.Type[] extraTypes, string path, IsolatedStorageFile isolatedStorageFolder)
    {
      T serializableObject = null;

      using (TextReader textReader = CreateTextReader(isolatedStorageFolder, path))
      {
        XmlSerializer xmlSerializer = CreateXmlSerializer(extraTypes);
        serializableObject = xmlSerializer.Deserialize(textReader) as T;

      }

      return serializableObject;
    }

    private static TextReader CreateTextReader(IsolatedStorageFile isolatedStorageFolder, string path)
    {
      TextReader textReader = null;

      if (isolatedStorageFolder == null)
        textReader = new StreamReader(path);
      else
        textReader = new StreamReader(new IsolatedStorageFileStream(path, FileMode.Open, isolatedStorageFolder));

      return textReader;
    }

    private static TextWriter CreateTextWriter(IsolatedStorageFile isolatedStorageFolder, string path)
    {
      TextWriter textWriter = null;

      if (isolatedStorageFolder == null)
        textWriter = new StreamWriter(path);
      else
        textWriter = new StreamWriter(new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isolatedStorageFolder));

      return textWriter;
    }

    private static XmlSerializer CreateXmlSerializer(System.Type[] extraTypes)
    {
      Type ObjectType = typeof(T);

      XmlSerializer xmlSerializer = null;

      if (extraTypes != null)
        xmlSerializer = new XmlSerializer(ObjectType, extraTypes);
      else
        xmlSerializer = new XmlSerializer(ObjectType);

      return xmlSerializer;
    }

    private static void SaveToDocumentFormat(T serializableObject, System.Type[] extraTypes, string path, IsolatedStorageFile isolatedStorageFolder)
    {
      using (TextWriter textWriter = CreateTextWriter(isolatedStorageFolder, path))
      {
        XmlSerializer xmlSerializer = CreateXmlSerializer(extraTypes);
        xmlSerializer.Serialize(textWriter, serializableObject);
      }
    }

    #endregion
  }
}