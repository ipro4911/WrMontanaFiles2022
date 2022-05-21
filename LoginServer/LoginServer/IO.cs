// Decompiled with JetBrains decompiler
// Type: LoginServer.IO
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace LoginServer
{
  public static class IO
  {
    public static string path;

    public static string workingDirectory
    {
      get
      {
        return Application.StartupPath;
      }
    }

    public static string ReadValue(string section, string value, bool takelast = true)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(IO.path);
        xmlDocument.DocumentElement.SelectSingleNode(section);
        XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName(value);
        if (elementsByTagName.Count > 1)
          Log.WriteLine("There are more nodes with the name " + section + "/" + value);
        return elementsByTagName[takelast ? elementsByTagName.Count - 1 : 0].InnerText;
      }
      catch (Exception ex)
      {
        Log.WriteError("Error while reading " + section + " [" + value + "]");
      }
      return "0";
    }

    public static string ReadAttribute(string section, string value, string subvalue)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(IO.path);
        return xmlDocument.DocumentElement.SelectNodes(section + "/" + value).Cast<XmlElement>().First<XmlElement>().Attributes.Cast<XmlAttribute>().Where<XmlAttribute>((Func<XmlAttribute, bool>) (r => r.Name.ToLower() == subvalue.ToLower())).First<XmlAttribute>().Value;
      }
      catch (Exception ex)
      {
        Log.WriteError("Error while reading " + section + " [" + value + " _ " + subvalue + "]");
      }
      return (string) null;
    }
  }
}
