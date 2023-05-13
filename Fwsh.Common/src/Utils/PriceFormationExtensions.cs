namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class PriceFormationExtensions
{
    static int PriceMarginPercent => PriceFormation.PriceMarginPercent;
    static int MaxDiscountPercent => PriceFormation.MaxDiscountPercent;


    public static int WithMargin (this int price)
    {
        return (int)Math.Ceiling((PriceMarginPercent + 100) * (double)price / 100);
    }

    public static int WithDiscountFor (this int price, Customer customer)
    {
        int discount = 100 - Math.Min(MaxDiscountPercent, customer.DiscountPercent);
        return (int)Math.Ceiling(discount * (double)price / 100);
    }

    public static double DefaultPrice (this ResourceQuantity res) 
    {
        return (res.SlotName == SlotNames.Decor)? PriceFormation.DefaultDecorPrice :
               (res.SlotName == SlotNames.Fabric)? PriceFormation.DefaultFabricPrice : 
                0;
    }
}
