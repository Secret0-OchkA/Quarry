using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Order
    {
        protected Order() { }


        public Guid Id { get; protected set; }

        public Person Driver { get; protected set; }

        public string CarNumber { get; protected set; } = string.Empty;
        public double CarWeight { get; protected set; }
        public double CargoWeight { get; protected set; } = 0;

        public double ContractCargoWeight { get; protected set; }
    
        public string Supplier { get; protected set; } = string.Empty;
        public string Customer { get; protected set; } = string.Empty;

        public OrderActiveStates activeStates { get; protected set; } = OrderActiveStates.NoActive;
        public OrderPaymentStatus paymentStatus { get; protected set; } = OrderPaymentStatus.NoPaid;
    }

    [Owned]
    public class Person
    {
        public string FName { get; protected set; } = string.Empty;
        public string LName { get; protected set; } = string.Empty;
        public string FathName { get; protected set; } = string.Empty;

        protected Person() { }
        public Person(string FName,string LName, string FathName = "")
        {
            this.FName = FName;
            this.LName = LName;
            this.FathName = FathName;
        }
    }

    public enum OrderActiveStates
    {
        Unknow,
        NoActive,
        Active,
        Archive,
    }

    public enum OrderPaymentStatus
    {
        Unknow,
        NoPaid,
        Paid,
    }
}
