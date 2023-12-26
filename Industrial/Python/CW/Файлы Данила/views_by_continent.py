import pandas as pd
import matplotlib.pyplot as plt
import pycountry_convert as pc
import json


def views_by_continent(data, document_id):
    # Function to get continent from country code using pycountry_convert
    def country_code_to_continent(country_code):
        try:
            continent_code = pc.country_alpha2_to_continent_code(country_code)
            return pc.convert_continent_code_to_continent_name(continent_code)
        except:
            return "Unknown"  # for codes that are not found or invalid

    # Filter data for the specific document_id
    data_for_document = data[data['env_doc_id'] == document_id].copy()

    # Apply the function to your DataFrame
    data_for_document.loc[:, 'continent'] = data_for_document['visitor_country'].apply(
        country_code_to_continent)

    # Group by continent and count views
    views_by_continent = data_for_document.groupby(
        "continent").size().sort_values(ascending=False)

    # Plot the views by continent
    views_by_continent.plot(kind="bar", figsize=(9, 7), title="Views by Continent")
    plt.show()