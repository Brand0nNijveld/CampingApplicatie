import json

pixelsPerMeter = 6.25
inserts = []
# Step 1: Load the JSON data from the file
with open("path-data.json", "r") as file:
    data = json.load(file)

print("INSERT INTO intersections (ID, PositionX, PositionY) VALUES")

verticeData = data["vectorNetwork"]["vertices"]
vertices = []
for index, obj in enumerate(verticeData):
    x = (obj["x"] + 43) / pixelsPerMeter
    y = (obj["y"] + 139 ) / pixelsPerMeter
    query = f"({index}, {x}, {y})"
    if index == len(verticeData) - 1:
        query += ";"
    else:
        query += ","
    
    print(query)
    vertices.append(index)

print("\nINSERT INTO roads (Intersection1_ID, Intersection2_ID) VALUES")
segments = data["vectorNetwork"]["segments"]
visited = set()
for index, obj in enumerate(segments):
    start = obj["start"]
    end = obj["end"]
    if (start, end) in visited or (end, start) in visited:
        continue

    query = f"({start}, {end})"
    if index == len(segments) - 1:
        query += ";"
    else:
        query += ","

    print(query)

    visited.add((start, end))