// Decompiled with JetBrains decompiler
// Type: LoginServer.Statment
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System.Collections.Generic;

namespace LoginServer
{
  public class Statment
  {
    public readonly Dictionary<string, object> parameters = new Dictionary<string, object>();

    public string query { get; set; }

    public Statment(string query)
    {
      this.query = query;
    }

    public void AddValue(string key, object value)
    {
      if (this.parameters.ContainsKey(key))
        return;
      this.parameters.Add(key, value);
    }
  }
}
