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
    class Decoder
    {
        // Fields
        private OrderClass order;

        // Constructor for Decoder Object
        public Decoder(string encoded)
        {
            // Split the encoded string by comma delimiter
            string[] strings = encoded.Split(',');

            // Set the parts of the strings array to their respective variables
            string senderID = strings[0];
            int cardNo = int.Parse(strings[1]);
            int amount = int.Parse(strings[2]);

            // Call the OrderClass constructor to set the order variable
            this.order = new OrderClass(senderID, cardNo, amount);
        }

        // Getter
        public OrderClass getOrder()
        {
            return this.order;
        }
    }
}