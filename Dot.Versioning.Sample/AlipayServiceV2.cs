﻿using System;

namespace Dot.Versioning.Sample
{
    [Versioning(Named = "alipay.trade.barcode.pay", Version = "2.0")]
    public class AlipayServiceV2 : IAlipayService
    {
        public void Display()
        {
            Console.WriteLine($"{this.GetType().FullName}");
        }
    }
}
