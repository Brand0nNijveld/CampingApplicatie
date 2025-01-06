using CampingApplication.Business;
using CampingApplication.Business.PathService;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PathRepository : IPathRepository
    {
        private DBConnection connection;

        public PathRepository(DBConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Graph> GetPath()
        {
            Graph graph = new Graph();
            Dictionary<int, Node> nodes = [];

            string intersectionsQuery = "SELECT ID, PositionX, PositionY FROM intersections;";
            string roadsQuery = "SELECT * FROM roads;";

            try
            {

                using (var conn = await connection.GetConnectionAsync())
                {
                    using (MySqlCommand command = new MySqlCommand(intersectionsQuery, conn))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("ID");
                                int posX = reader.GetInt32("PositionX");
                                int posY = reader.GetInt32("PositionY");
                                Debug.WriteLine(id);
                                nodes.Add(id, new(id, posX, posY));
                            }
                        }
                    }

                    using (MySqlCommand command = new MySqlCommand(roadsQuery, conn))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                int id1 = reader.GetInt32("Intersection1_ID");
                                int id2 = reader.GetInt32("Intersection2_ID");
                                Debug.WriteLine(id1);

                                graph.ConnectNodes(nodes[id1], nodes[id2]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return graph;
        }
    }
}
