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

namespace A2_A3
{
    class OrderClass
    {
        // Private fields
        private string senderID;
        private int cardNo;
        private int amount;

        // Constructor for the OrderClass object
        public OrderClass(string senderID, int cardNo, int amount)
        {
            this.senderID = senderID;
            this.cardNo = cardNo;
            this.amount = amount;
        }

        // Getters
        public string getSenderID()
        {
            return this.senderID;
        }

        public int getCardNo()
        {
            return this.cardNo;
        }

        public int getAmount()
        {
            return this.amount;
        }
    }
}