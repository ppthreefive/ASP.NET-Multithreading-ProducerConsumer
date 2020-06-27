/* Author: Phillip Pham
 * Instructor: Dr. Yinong Chen
 * Course: CSE445 Section: 41633
 * 
 * Program Title: Assignment 2&3, Simplified E-Commerce Application
 * Program Description: 
 *          Creating a simplified e-commerce system consisting of multiple chicken retailers (clients) and a single chicken farm (server).
 *          The purpose of this project is to be familiar with the concepts such as distributed computing, multithreading, thread definition, 
 *          creation, management, synchronization, cooperation, event-driven programming, client and server architecture, 
 *          service execution model of creating a new thread for each request, the performance of parallel computing, 
 *          and the impact of multi-core processors to multithreading programs with complex coordination and cooperation.
 */

using System;
using System.Threading;

namespace A2_A3
{
    class Retailer
    {
        // We will use this static variable to generate random numbers for our order amounts
        public static Random rng = new Random(); 
        // This static delegate event will signal to our ChickenFarm that an order has been generated
        public static event orderGeneratedEvent orderGenerated; 

        // This is the default method for our retailer threads, will always run
        public void retailerFunc()
        {
            // This thread will stay alive until the ChickenFarm goes inactive
            while (Program.farmThreadActive)
            {
                // Generates an order every 1000 milliseconds, regardless of sale
                Thread.Sleep(1000);
                generateOrder(Thread.CurrentThread.Name);
            }
        }

        /* This method generates an order with a random card number and random quantity. 
         * Then generates an OrderClass object which then is encoded to a string, which gets placed into our buffer.
         * Finally, it then signals to the ChickenFarm that an order has been generated using the delegate event.
         */
        private void generateOrder(string senderID)
        {
            // Generate random valid numbers for the card number, and quantity
            int cardNo = rng.Next(5000, 7000);
            int amount = rng.Next(25,100);

            // Generate OrderClass object, then encode to a string using our Encoder object
            OrderClass order = new OrderClass(senderID, cardNo, amount);
            Encoder encode = new Encoder(order);
            string encoded = encode.getEncodedString();

            // Print the generation confirmation
            Console.WriteLine("Store {0}'s order generated, Time-stamp: {1}", senderID, DateTime.Now.ToString("hh:mm:ss"));

            // Place the encoded string into our buffer
            Program.buffer.setOneCell(encoded);

            // Signal to our ChickenFarm that the order has been generated using our delegate event
            orderGenerated();
        }

        // This method prints a confirmation that an order has been processed with total price
        public void orderProcessed(string senderID, int total, int price, int quantity)
        {
            Console.WriteLine("Store {0}'s order processed. Total: $" + total + ", Ind. Price: {1}, Quantity: {2}.", senderID,
                                price, quantity);
        }

        // This method is invoked everytime a pricecut is generated, and causes an immediate order generation from retailer threads
        public void chickenOnSale(int p, string senderID)
        {
            // Event handler    
            Console.WriteLine("----------------------------------------------------------------------");
            Console.WriteLine("Store {0} chickens are on sale: as low as ${1} each (Price Cut Event)", senderID, p);
            Console.WriteLine("----------------------------------------------------------------------");
            generateOrder(senderID);
        }
    }
}