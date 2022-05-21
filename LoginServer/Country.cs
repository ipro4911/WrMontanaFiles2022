// Decompiled with JetBrains decompiler
// Type: Country
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System;

public class Country
{
  private string code;
  private string name;

  ~Country()
  {
    GC.Collect();
  }

  public Country(string code, string name)
  {
    this.code = code;
    this.name = name;
  }

  public string getCode()
  {
    return this.code;
  }

  public string getName()
  {
    return this.name;
  }
}
