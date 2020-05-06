using System;

namespace Dot.Versioning.Sample
{
    [Versioning(Named = "alipay.trade.barcode.pay", Version = "1.0")]
    public class AlipayServiceV1 : IAlipayService
    {
        public void Display()
        {
            Console.WriteLine($"{this.GetType().FullName}");
        }
    }
}
