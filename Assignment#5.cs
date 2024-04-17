using System;
using System.Collections.Generic;

namespace Post
{
    // Base class representing a mail item
    public abstract class Mail
    {
        // Attributes common to all types of mail
        public double Weight { get; }
        public bool Express { get; }
        public string Destination { get; }

        // Constructor
        public Mail(double weight, bool express, string destination)
        {
            Weight = weight;
            Express = express;
            Destination = destination;
        }

        // Abstract method for calculating postage
        public abstract double CalculatePostage();

        // Method to check if mail is valid
        public virtual bool IsValid()
        {
            return !string.IsNullOrEmpty(Destination);
        }

        // Method to display mail details
        public abstract void Display();
    }

    // Derived class representing a letter
    public class Letter : Mail
    {
        public string Format { get; }

        // Constructor
        public Letter(double weight, bool express, string destination, string format)
            : base(weight, express, destination)
        {
            Format = format;
        }

        // Override method to calculate postage for a letter
        public override double CalculatePostage()
        {
            double baseFare = (Format == "A4") ? 2.50 : 3.50;
            double postage = baseFare + Weight / 1000.0; // Convert grams to kilograms
            return (Express) ? 2 * postage : postage;
        }

        // Override method to display letter details
        public override void Display()
        {
            Console.WriteLine($"Letter\nWeight: {Weight} grams\nExpress: {(Express ? "yes" : "no")}\nDestination: {Destination}\nPrice: ${CalculatePostage()}\nFormat: {Format}");
        }
    }

    // Derived class representing a parcel
    public class Parcel : Mail
    {
        public double Volume { get; }

        // Constructor
        public Parcel(double weight, bool express, string destination, double volume)
            : base(weight, express, destination)
        {
            Volume = volume;
        }

        // Override method to calculate postage for a parcel
        public override double CalculatePostage()
        {
            double postage = 0.25 * Volume + Weight / 1000.0; // Convert grams to kilograms
            return (Express) ? 2 * postage : postage;
        }

        // Override method to check if parcel is valid
        public override bool IsValid()
        {
            return base.IsValid() && Volume <= 50;
        }

        // Override method to display parcel details
        public override void Display()
        {
            Console.WriteLine($"Parcel\nWeight: {Weight} grams\nExpress: {(Express ? "yes" : "no")}\nDestination: {Destination}\nPrice: ${CalculatePostage()}\nVolume: {Volume} liters");
        }
    }

    // Derived class representing an advertisement
    public class Advertisement : Mail
    {
        // Constructor
        public Advertisement(double weight, bool express, string destination)
            : base(weight, express, destination)
        {
        }

        // Override method to calculate postage for an advertisement
        public override double CalculatePostage()
        {
            double postage = 5.0 * Weight / 1000.0; // Convert grams to kilograms
            return (Express) ? 2 * postage : postage;
        }

        // Override method to display advertisement details
        public override void Display()
        {
            Console.WriteLine($"Advertisement\nWeight: {Weight} grams\nExpress: {(Express ? "yes" : "no")}\nDestination: {Destination}\nPrice: ${CalculatePostage()}");
        }
    }

    // Class representing a mailbox
    public class Box
    {
        private List<Mail> mails;

        // Constructor
        public Box(int capacity)
        {
            mails = new List<Mail>(capacity);
        }

        // Method to add mail to the mailbox
        public void AddMail(Mail mail)
        {
            mails.Add(mail);
        }

        // Method to calculate total postage
        public double Stamp()
        {
            double totalPostage = 0;
            foreach (var mail in mails)
            {
                totalPostage += mail.IsValid() ? mail.CalculatePostage() : 0;
            }
            return totalPostage;
        }

        // Method to count invalid mails
        public int InvalidMails()
        {
            int count = 0;
            foreach (var mail in mails)
            {
                if (!mail.IsValid())
                    count++;
            }
            return count;
        }

        // Method to display contents of the mailbox
        public void Display()
        {
            foreach (var mail in mails)
            {
                mail.Display();
                if (!mail.IsValid())
                    Console.WriteLine("(Invalid mail)");
            }
        }
    }

    // Main class
    public class Post
    {
        public static void Main(string[] args)
        {
            // Creation of a mailbox 
            // The maximum size of a box is 30
            Box box = new Box(30);

            Letter letter1 = new Letter(200, true, "Chemin des Acacias 28, 1009 Pully", "A3");
            Letter letter2 = new Letter(800, false, "", "A4"); // invalid

            Advertisement adv1 = new Advertisement(1500, true, "Les Moilles 13A, 1913 Saillon");
            Advertisement adv2 = new Advertisement(3000, false, ""); // invalid

            Parcel parcel1 = new Parcel(5000, true, "Grand rue 18, 1950 Sion", 30);
            Parcel parcel2 = new Parcel(3000, true, "Chemin des fleurs 48, 2800 Delemont", 70); // invalid parcel

            box.AddMail(letter1);
            box.AddMail(letter2);
            box.AddMail(adv1);
            box.AddMail(adv2);
            box.AddMail(parcel1);
            box.AddMail(parcel2);

            Console.WriteLine("The total amount of postage is " + box.Stamp());
            box.Display();
            Console.WriteLine("The box contains " + box.InvalidMails() + " invalid mails");
        }
    }
}
