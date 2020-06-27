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

using System.Threading;

namespace A2_A3
{
    class MultiCellBuffer
    {
        // Declare our buffer and initialize the number of elements to 0
        private string[] buffer;
        private int elements = 0;

        // We are choosing the number of cells to be 2, max number N is 5.
        private const int N = 5;
        private const int n = 2;
        private Semaphore writes;
        private Semaphore reads;

        // Constructor for MultiCellBuffer object
        public MultiCellBuffer()
        {
            // Lock the resource so nothing else can mess with it yet
            lock (this)
            {
                // Initialize our Semaphores and resources
                writes = new Semaphore(n, n);
                reads = new Semaphore(n, n);
                buffer = new string[n];

                // Initialize our buffer cells as null
                for (int i = 0; i < n; i++)
                {
                    buffer[i] = null;
                }
            }
        }

        // Writes data into an available cell in the MultiCellBuffer object.
        public void setOneCell(string data)
        {
            // Try to acquire the write resource
            writes.WaitOne();

            // Lock and enter critical section
            lock (this)
            {
                // Thread must enter wait stage if buffer is full
                while (elements == n)
                {
                    Monitor.Wait(this);
                }

                // Goes through the cells and exits as soon as it finds one that is empty.
                for (int i = 0; i < n; i++)
                {
                    // Checks if the cell is empty first, then writes the data if true.
                    if (buffer[i] == null) 
                    {
                        buffer[i] = data;
                        elements++;

                        // We have written the data, so we can break the loop
                        break;
                    }
                }

                // Release the write resource
                writes.Release();

                // Communicate with other threads that resource is free
                Monitor.Pulse(this);
            }
        }

        // Reads data from a valid cell in the MultiCellBuffer object.
        public string getOneCell()
        {
            // Try to acquire read resource
            reads.WaitOne();

            string output = "";

            // Lock and enter critical section
            lock (this)
            {
                // No cells have any data, so the thread must wait
                while (elements == 0)
                {
                    Monitor.Wait(this);
                }

                // Checks if there is actual data in the buffer cell
                for (int i = 0; i < n; i++)
                {
                    if (buffer[i] != null) 
                    {
                        // Grab the data and attach to our output
                        output = buffer[i];

                        // Reset the buffer cell to null and decrement element counter
                        buffer[i] = null;
                        elements--;

                        // We have acquired the data, so we can break the loop
                        break;
                    }
                }

                // Release the read resource
                reads.Release();

                // Communicate with the other threads that resource is free
                Monitor.Pulse(this);
            }

            // Return the data
            return output;
        }
    }
}