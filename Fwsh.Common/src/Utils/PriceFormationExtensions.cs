namespace Fwsh.Common;

using System;
using System.Collections.Generic;

public static class PriceFormationExtensions
{
    public static int PriceMarginPercent = 45;
    public static int MaxDiscountPercent = 30;


    public static int WithMargin (this int price)
    {
        return (int)Math.Ceiling((PriceMarginPercent + 100) * (double)price / 100);
    }

    public static int WithDiscountFor (this int price, Customer customer)
    {
        int discount = 100 - Math.Min(MaxDiscountPercent, customer.DiscountPercent);
        return (int)Math.Ceiling(discount * (double)price / 100);
    }
}
