namespace NewRelish
{
    class HealthServices
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ContactDesignation { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTelephone { get; set; }


    }
    class HealthStates
    {

        public string State { get; set; }

    }
    class HealthLgas
    {
        public string State { get; set; }
        public string Lga { get; set; }

    }
    class HealthServiceDetail
    {
        public bool IsChecked { get; set; }

        public string ServiceDetail { get; set; }
        public string ServiceDetailID { get; set; }

        public string ServiceItemCost { get; set; }

    }

    public class Serviceoptions
    {
        public string ServiceDetail { get; set; }

        public string ServiceItemCost { get; set; }

        public override string ToString()
        {
            return ServiceDetail + " :::" + ServiceItemCost;
        }
    }
    class Services
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ContactDesignation { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTelephone { get; set; }

        // public override string ToString()
        // {
        //     return Contact + " " + Name + " " + ContactTelephone;
        // }
    }

    public class Doctor
    {
        public string DoctorName { get; set; }
        public string YearMBBS { get; set; }
        public string MDCNFolioNo { get; set; }
        public string Institution { get; set; }
        public string State { get; set; }
        public string Lga { get; set; }

    }
    public enum ServiceList
    {
        Gym,
        Ambulance,
        Pharmacy,
        Hospital

    }
    public class Abmulance
    {

        public string Name { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ContactDesignation { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTelephone { get; set; }

        public override string ToString()
        {
            return Contact + " " + Name + " " + ContactTelephone;
        }
    }
    public class Gym
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ContactDesignation { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTelephone { get; set; }

        public override string ToString()
        {
            return Contact + " " + Name + " " + ContactTelephone;
        }
    }
    public class Hospital { }



}