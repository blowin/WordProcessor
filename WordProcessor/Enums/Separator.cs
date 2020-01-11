using FlexEnum.DataTypes.Enums;

namespace WordProcessor.Enums
{
  public sealed class Separator : BaseEnum<string>
  {
    public static readonly Separator NewLine = new Separator("Новая строка");
    public static readonly Separator Custom = new Separator("Ручной ввод");

    private Separator(string val) : base(val)
    {
    }
  }
}