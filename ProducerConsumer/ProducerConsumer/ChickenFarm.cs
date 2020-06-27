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
    class ChickenFarm
    {
        // We are declaring a static random variable to allow us to generate random pricing
        static Random rng = new Random();
        public static event priceCutEvent priceCut;

        // Need static variables to keep track of chicken prices, the number of price cuts, and which store is having the sale (n)
        private static int cutCount = 0;
        private static int chickenPrice = 20;
        private static int n = 0;

        // This method receives an order from the buffer, decodes using a Decoder object, and then passes it to an OrderProcessor object
        public void receiveOrder()
        {
            // Grabs an order from the buffer in encoded string format
            string encodedOrder = Program.buffer.getOneCell();

            // Initialize our Decoder and OrderProcessing objects
            Decoder decode = new Decoder(encodedOrder);
            OrderProcessing processor = new OrderProcessing();

            // Decode the string back into an OrderClass object and pass it to our OrderProcessing thread
            OrderClass order = decode.getOrder();
            Thread t = new Thread(() => processor.process(order, getPrice()));

            // Start the OrderProcessing thread
            t.Start();
        }

        // This method returns the current chickenPrice
        public int getPrice()
        {
            return chickenPrice;
        }

        // This method changes the price of chickens to the given int parameter
        public static void changePrice(int price)
        {
            // We want to reset the retailer number if it's equal to the length of our retailer thread array to avoid crashing
            if (n == Program.retailers.Length)
            {
                n = 0;
            }

            // If the price is lower then the current price of chicken, then it's a price cut
            if (price < chickenPrice) 
            {
                if (priceCut != null) // there is at least a subscriber
                {
                    // Set the price of the chicken to our new price
                    chickenPrice = price;

                    priceCut(price, Program.retailers[n].Name); // emit event to subscribers

                    // Increment our price cut counter
                    cutCount++;

                    // Change the store number
                    n++;
                } 
            }
            else
            {
                // Set the price of the chicken to our new price
                chickenPrice = price;
            }
        }

        // This is our default method that will run when the thread starts from Program.cs
        public void farmerFunc()
        {
            // Will run until we reach 10 price cuts
            while(cutCount < 10)
            {
                // Iterates every 500 milliseconds
                Thread.Sleep(500);

                // Generate a random price using PricingModel() method
                int p = PricingModel();   
                
                // Change the chicken price to our price generated from PricingModel()
                ChickenFarm.changePrice(p);
            }

            // Once the loop is complete and 10 price cuts have happened, we can terminate our threads
            Program.farmThreadActive = false;
        }

        // Our pricing model generates a random number from 5 to 30, ensuring that it can drop price and increase
        public int PricingModel()
        {
            return rng.Next(15, 30);
        }
    }
}