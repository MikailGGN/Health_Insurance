namespace NewRelish
{
    class Subscriber
    {
        public string Mobilenumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string Sex { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }

        public string photo { get; set; }


    }

    class Presciption
    {
        public string Mobilenumber { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string Date { get; set; }
        public string Doctor { get; set; }
        public int ClinicID { get; set; }

    }
    class Charges
    {
        public int ProviderID { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDescription { get; set; }
        public decimal ServiceCharge { get; set; }
    }

    class Symptoms
    {
        public string Mobilenumber { get; set; }
        public string Syms { get; set; }
        public string Date { get; set; }
    }

    class SubscriberBilling
    {

        public string Mobilenumber { get; set; }
        public int ProviderID { get; set; }
        public string ServiceType { get; set; }
        public string ServiceDescription { get; set; }
        public decimal ServiceCharge { get; set; }
        public string Date { get; set; }


    }
}