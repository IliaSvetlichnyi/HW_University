from views_by_country import views_by_country
from views_by_continent import views_by_continent
from views_by_browser_clear import views_by_browser_clear
from views_by_visitor_useragent import views_by_visitor_useragent
from reader_profile import reader_profile
from also_like_graph import create_graph
import json
import pandas as pd

# Initialize an empty list to store JSON objects
json_data = []

# Load each JSON object from each line
with open('/Users/ilya/Desktop/GitHub_Repositories/HW_University/Industrial/Python/CW/datasets/sample_100k_lines.json', 'r') as file:
    for line in file:
        try:
            json_object = json.loads(line)
            json_data.append(json_object)
        except json.JSONDecodeError:
            print("Error decoding JSON from line:", line)

# Convert the list of JSON objects to a DataFrame
df = pd.DataFrame(json_data)


# Call the function and show the plot
document_id = '140228202800-6ef39a241f35301a9a42cd0ed21e5fb0'
# views_by_country(df, document_id) # 2_a
# views_by_continent(df, document_id)  # 2_b
# views_by_visitor_useragent(df, document_id) # 3_a
# views_by_browser_clear(df, document_id) # 3_b
# print(reader_profile(df, document_id))  # 4
create_graph(document_id, df)
