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

namespace A2_A3
{
    class OrderProcessing
    {
        // This static event signals to the retailer that the order has been processed
        public static event orderProcessedEvent orderProcessed;

        // I want the tax to simply be 10% and shipping costs to always be $5.00
        // So I declare and initialize them as constants here
        private const double TAX = 0.1;
        private const double SHIPPING_HANDLING = 5.0;

        /* This method will process the order and payment by checking the card number validity first, and then calculates total price.
         *  Once total price calculated, the delegate event orderProcessed signals to ChickenFarm that the thread has completed 
         *  order processing. */
        public void process(OrderClass order, int unitPrice)
        {
            // Check first if the card number is valid
            if (!checkCreditCardNumber(order.getCardNo()))
            {
                // Write an error to console if there's an error and then return
                Console.WriteLine("Card number {0} is not valid.", order.getCardNo());
                return;
            }
            else
            {
                // Calculate the subtotal first
                double subtotal = (unitPrice * order.getAmount());

                // Then calculate the tax amount
                double taxes = (subtotal * TAX);

                // Subtotal + Taxes + shipping and handling
                int total = Convert.ToInt32(subtotal + taxes + SHIPPING_HANDLING);

                // Signal to the retailer that the order has been processed
                orderProcessed(order.getSenderID(), total, unitPrice, order.getAmount());
            }
        }

        // This method simply returns true if the card number is within valid range, or false if not
        private Boolean checkCreditCardNumber(int cardNo)
        {
            if (cardNo <= 7000 && cardNo >= 5000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
