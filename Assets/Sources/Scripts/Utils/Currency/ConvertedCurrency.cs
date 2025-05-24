namespace FunnyBlox.Game
{
  [System.Serializable]
  public class ConvertedCurrency
  {
    public float ConvertedAmount; //The raw converted amount
    public float AmountAfterFees; //The amount after the fees cut
    public float Fees;            //Conversion fees cut
  }
}