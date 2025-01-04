import json
import mysql
from mysql.connector import Error

try:
    connection = mysql.connector.connect(
        host='localhost',
        user='root',
        password='',
        database='campingapplicatie'
    )

    pixelsPerMeter = 6.25
    query = "INSERT INTO campingspot(CampingID, SpotNr, PositionX, PositionY)\n"
    query += "VALUES\n"
    inserts = []
    # Step 1: Load the JSON data from the file
    with open("spot-data.json", "r") as file:
        data = json.load(file)

    # Step 2: Iterate over the array
    query_parts = []  # Use a list to collect query fragments
    for index, obj in enumerate(data):
        x = obj["x"] / pixelsPerMeter
        y = obj["y"] / pixelsPerMeter

        # Append the query fragment
        query_parts.append(f"(1, {index + 1}, {x:.2f}, {y:.2f})")  # Format x and y to 2 decimal places

    # Join all parts with commas and newline at the end
    query = "INSERT INTO campingspot (CampingID, SpotNr, PositionX, PositionY) VALUES\n" + ",\n".join(query_parts) + ";"

    print(query)

    if connection.is_connected():
        cursor = connection.cursor()
        print(query)
        cursor.execute(query)
        connection.commit()
except Error as e:
    print(f"Error: {e}")
finally:
    if connection.is_connected():
        cursor.close()
        connection.close()
