using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FunnyBlox.Game
{
  public class CurrencyManager : MonoBehaviour
  {
    [Header("Main")] public List<Currency> Currencies;

    [SerializeField] private CurrencyManagerData currencyData;

    public static CurrencyManager instance;
    private CurrencyTextManager currencyTextManager;

    [Header("Currency Converter")]
    [Range(0, 99)]
    [Tooltip("Fees in percentage when converting money to another currency")]
    public float ConversionFeesPercentage = 0;

    private void OnEnable()
    {
      SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnDisable()
    {
      SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void Awake()
    {
      if (instance) Destroy(gameObject);
      else instance = this;
      DontDestroyOnLoad(gameObject);

      Currencies = new List<Currency>();
      for (int i = 0; i < currencyData.Currencies.Count; i++)
      {
        Currency currency = new Currency();

        currency.SaveName = currencyData.Currencies[i].SaveName;
        currency.Debt = currencyData.Currencies[i].Debt;
        currency.Ratio = currencyData.Currencies[i].Ratio;

        if (!PlayerPrefs.HasKey(currency.SaveName))
        {
          currency.Amount = currencyData.Currencies[i].Amount;
          PlayerPrefs.SetFloat(currency.SaveName, currency.Amount);
        }
        else
          currency.Amount = PlayerPrefs.GetFloat(currency.SaveName, currencyData.Currencies[i].Amount);

        Currencies.Add(currency);
      }

      GetActiveTextManager();
    }

    /// <summary>
    /// Adds amount to the target currency (to remove amount then use a negative amount (i.e -10))
    /// </summary>
    /// <param name="currencySaveName"></param>
    /// <param name="amount"></param>
    public void UpdateAmount(string currencySaveName, float amount)
    {
      //Loop through all the currencies
      for (int i = 0; i < Currencies.Count; i++)
      {
        //If the current currency save name equals the target currency save name
        if (Currencies[i].SaveName == currencySaveName)
        {
          Currencies[i].Amount += amount; //Add the amount to the target currency's amount

          //if this currency doesn't allow dept and the amount is less than 0
          if (!Currencies[i].Debt && Currencies[i].Amount < 0)
            Currencies[i].Amount = 0; //Then force the amount to be 0
          //Other than that then this means that debt is allowed so the currency can be a negative value

          //Save the amount of the target currency
          PlayerPrefs.SetFloat(currencySaveName, Currencies[i].Amount);
        }
      }

      if (currencyTextManager)
        currencyTextManager.UpdateSpecificUI(currencySaveName); //Update all the texts of the target currency
    }

    public void WipeCurrency(string currencySaveName)
    {
      for (int i = 0; i < Currencies.Count; i++)
      {
        //If the current currency save name equals the target currency save name
        if (Currencies[i].SaveName == currencySaveName)
        {
          Currencies[i].Amount = 0f;
          PlayerPrefs.SetFloat(currencySaveName, Currencies[i].Amount);
        }
      }
    }

    /// <summary>
    /// Returns the currency amount (float) using a currency saveID
    /// </summary>
    /// <param name="currencySaveName"></param>
    /// <returns></returns>
    public float GetCurrencyAmount(string currencySaveName)
    {
      return GetCurrencyData(currencySaveName).Amount; //Get only the amount of the currency data
    }

    /// <summary>
    /// Returns all the currency data using a currency saveID
    /// </summary>
    /// <param name="currencySaveName"></param>
    /// <returns></returns>
    public Currency GetCurrencyData(string currencySaveName)
    {
      Currency currency = null; //Create a new temporary currency

      //Loop through all the currencies
      for (int i = 0; i < Currencies.Count; i++)
      {
        //If the current currency save name equals the target save name
        if (Currencies[i].SaveName == currencySaveName)
        {
          currency = Currencies[i]; //Then set the temporary currency to the target currency
        }
      }

      return
        currency; //Finally return this currency (if the save name doesn't equal any of the currencies save name then the function will return null)
    }

    public int GetCurrencyIndex(Currency currency)
    {
      return Currencies.IndexOf(currency);
    }

    /// <summary>
    /// Calculates the data of the Currency Conversion (ConvertedAmount, AmountAfterFees, Fees)
    /// </summary>
    /// <param name="From_Currency"></param>
    /// <param name="To_Currency"></param>
    /// <param name="Amount"></param>
    /// <returns></returns>
    public ConvertedCurrency CalculateCurrencyConversionData(string From_Currency, string To_Currency, float Amount)
    {
      float primaryRatio = GetCurrencyData(From_Currency).Ratio; //The From_Currency Ratio
      float targetRatio = GetCurrencyData(To_Currency).Ratio; //The To_Currency Ratio
      ConvertedCurrency
        currency = new ConvertedCurrency(); //Create a new ConvertedCurrency variable to hold the data

      if (primaryRatio >=
          targetRatio) //If the Ratio of the From_Currency is greater than / equal the To_Currency (To_Currrency is more worth than the From_Currency)
        currency.ConvertedAmount =
          Amount / (primaryRatio /
                    targetRatio); //Divide the Amount by the proper Ratio (the greater/the smaller value) the set it to the ExpectedAmount
      else
        currency.ConvertedAmount =
          Amount * (targetRatio /
                    primaryRatio); //Multiply the Amount by the proper Ratio (the greater/the smaller value) the set it to the ExpectedAmount
      //Multiplication is used to increase the value

      currency.Fees = currency.ConvertedAmount * ConversionFeesPercentage / 100; //Get the amount of the fees only

      if (ConversionFeesPercentage > 0) //If there's a conversion fee
        currency.AmountAfterFees =
          currency.ConvertedAmount * (100 - ConversionFeesPercentage) /
          100; //Then get the remaining value of the amount (i.e if conversionFees are 10 then get the other 90% of the amount)
      else
        currency.AmountAfterFees =
          currency
            .ConvertedAmount; //If no conversion fee, then just set the amountAfterFees the same as the ExpectedAmount value

      return currency; //Finally return the currency data
    }

    /// <summary>
    /// Exchanges given Amount from a currency to another (Converted Amount is calculated automatically)
    /// </summary>
    /// <param name="From_Currency"></param>
    /// <param name="To_Currency"></param>
    /// <param name="Amount"></param>
    public void ExchangeCurrency(string From_Currency, string To_Currency, float Amount)
    {
      ExchangeCurrency(From_Currency, To_Currency, Amount,
        CalculateCurrencyConversionData(From_Currency, To_Currency, Amount).ConvertedAmount);
    }

    /// <summary>
    /// Exchanges given Amount from a currency to another using the ConvertedAmount
    /// </summary>
    /// <param name="From_Currency"></param>
    /// <param name="To_Currency"></param>
    /// <param name="Amount"></param>
    /// <param name="ConvertedAmount"></param>
    public void ExchangeCurrency(string From_Currency, string To_Currency, float Amount, float ConvertedAmount)
    {
      //Check if there's enough money to convert the Required Amount
      //Or if the currency has debt enabled (therefor, the money will be converted all the time and the From_Currency will be in negative value
      if (GetCurrencyAmount(From_Currency) >= Amount || GetCurrencyData(From_Currency).Debt)
      {
        UpdateAmount(From_Currency, -Amount); //Remove the amount from the From_Currency
        UpdateAmount(To_Currency, ConvertedAmount); //Add the Converted Amount to the To_Currency
      }
    }


    private void OnLoadScene(Scene scene, LoadSceneMode mode)
    {
      GetActiveTextManager();
    }

    /// <summary>
    /// Searches for the TextManager in scene
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void GetActiveTextManager()
    {
      //Get the CurrencyTextManager from the scene (if it's not available then the variable will be null)
      currencyTextManager = FindFirstObjectByType<CurrencyTextManager>();

      //If the currencyTextManager is not null
      if (currencyTextManager)
        currencyTextManager.UpdateAllUI(); //Update all the UI in the TextManager
    }
  }
}