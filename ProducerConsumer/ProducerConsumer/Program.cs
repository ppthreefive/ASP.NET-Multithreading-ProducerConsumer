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
    // This will emit to all retailers that there is a price cut on chickens from the ChickenFarm using a delegate event
    public delegate void priceCutEvent(int pr, string senderID);

    // This will emit to the ChickenFarm that an order has been processed using a delegate event
    public delegate void orderProcessedEvent(string senderID, int total, int price, int quantity);

    // This will emit to the ChickenFarm that an order from a retailer has been generated using a delegate event
    public delegate void orderGeneratedEvent();

    class Program
    {
        /* We want to declare our global variables here. We need our MultiCellBuffer,
         * our retailer threads, and a condition variable that lets our retailer threads 
         * know if the Farm is still active. (The ChickenFarm must close after 10 price cuts) */
        public static MultiCellBuffer buffer;
        public static Thread[] retailers;
        public static Boolean farmThreadActive = true;

        static void Main(string[] args)
        {
            // Initialize our ChickenFarm
            ChickenFarm chicken = new ChickenFarm();

            // Initialize our Buffer
            buffer = new MultiCellBuffer();

            // Start our ChickenFarm thread
            Thread farmer = new Thread(new ThreadStart(chicken.farmerFunc));
            farmer.Start();
            
            // Initialize our Retailer
            Retailer chickenstore = new Retailer();

            // ChickenFarm communicates to the subscribed retailer when a price cut happens, then the retailer generates an order
            ChickenFarm.priceCut += new priceCutEvent(chickenstore.chickenOnSale);

            // Retailer communicates to the ChickenFarm when an order is generated, then the ChickenFarm processes the order
            Retailer.orderGenerated += new orderGeneratedEvent(chicken.receiveOrder);

            // Once the respective retailer gets confirmation of order processing, the order is printed
            OrderProcessing.orderProcessed += new orderProcessedEvent(chickenstore.orderProcessed);
            
            // Initialize 5 new threads for our retailers
            retailers = new Thread[5];

            for (int i = 0; i < 5; i++)
            {
                // Make 5 copies of our retailer into our 5 Threads
                retailers[i] = new Thread(new ThreadStart(chickenstore.retailerFunc));

                // Give our threads a name (1 through 5 respectively)
                retailers[i].Name = (i + 1).ToString();

                // Then start our 5 retailer threads  
                retailers[i].Start();
            }
        }
    }
}