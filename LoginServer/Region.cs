// Decompiled with JetBrains decompiler
// Type: Region
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

public class Region
{
  public string countryCode;
  public string countryName;
  public string region;

  public Region()
  {
  }

  public Region(string countryCode, string countryName, string region)
  {
    this.countryCode = countryCode;
    this.countryName = countryName;
    this.region = region;
  }

  public string getcountryCode()
  {
    return this.countryCode;
  }

  public string getcountryName()
  {
    return this.countryName;
  }

  public string getregion()
  {
    return this.region;
  }
}
