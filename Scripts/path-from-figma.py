import json

pixelsPerMeter = 6.25
inserts = []
# Step 1: Load the JSON data from the file
with open("path-data.json", "r") as file:
    data = json.load(file)

verticeData = data["vectorNetwork"]["vertices"]
vertices = []
for index, obj in enumerate(verticeData):
    x = (obj["x"] + 43) / pixelsPerMeter
    y = (obj["y"] + 139 ) / pixelsPerMeter
    print(f"var node{index} = new Node({index}, {x}, {y});")
    vertices.append(f"node{index}")

segments = data["vectorNetwork"]["segments"]
for index, obj in enumerate(segments):
    start = obj["start"]
    end = obj["end"]
    print(f"graph.ConnectNodes({vertices[start]}, {vertices[end]});")