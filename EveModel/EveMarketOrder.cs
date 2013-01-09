using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveMarketOrder : EveObject
    {


        public EveMarketOrder(IntPtr ptr)
            : base()
        {
            PointerToObject = ptr;
            price = this["price", false].GetValueAs<double>();
            volRemaining = this["volRemaining", false].GetValueAs<int>();
            typeID = this["typeID", false].GetValueAs<int>();
            range = this["range", false].GetValueAs<int>();
            orderID = this["orderID", false].GetValueAs<long>();
            volEntered = this["volEntered", false].GetValueAs<int>();
            minVolume = this["minVolume", false].GetValueAs<int>();
            bid = this["bid", false].GetValueAs<bool>();
            stationID = this["stationID", false].GetValueAs<int>();
            regionID = this["regionID", false].GetValueAs<int>();
            solarSystemID = this["solarSystemID", false].GetValueAs<int>();
            jumps = this["jumps", false].GetValueAs<int>();
        }



        public double price { get; private set; }
        public int volRemaining  { get; private set; }
        public int typeID { get; private set; }
        public int range { get; private set; }
        public long orderID { get; private set; }
        public int volEntered { get; private set; }
        public int minVolume { get; private set; }
        public bool bid { get; private set; }
        public int stationID { get; private set; }
        public int regionID { get; private set; }
        public int solarSystemID { get; private set; }
        public int jumps { get; private set; }
        public string _name { get; private set; }
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                    _name = Frame.Client.GetInvType(new object[] { typeID })["typeName"].GetValueAs<string>();
                return _name;
            }
        }

        //def BuyStuff(self, stationID, typeID, price, quantity, orderRange = None, minVolume = 1, duration = 0, useCorp = False)
        public void buy(int quant)
        {
            if (quant > volRemaining)
            {
                quant = volRemaining;
            }
            Frame.Client.GetService("marketQuote").CallMethod("BuyStuff", new object[] { stationID, typeID, price, quant });
        }

        public void buy()
        {

            Frame.Client.GetService("marketQuote").CallMethod("BuyStuff", new object[] { stationID, typeID, price, volRemaining });
        }

 //def SellStuff(self, stationID, typeID, itemID, price, quantity, duration = 0, useCorp = False, located = None):
        public void sell(int quant, EveItem itemsell)
        {
            if (quant > volRemaining)
            {
                quant = volRemaining;
            }
            if (quant > itemsell.Quantity)
            {
                quant = itemsell.Quantity;
            }
            Frame.Client.GetService("marketquote").CallMethod("SellStuff", new object[] { stationID, typeID, itemsell.ItemId, price, quant});
        }
    }
}