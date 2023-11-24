import pandas as pd
import matplotlib.pyplot as plt


def views_by_country(data, document_id):
    # Filter data for the specific document_id
    data_for_document = data[data['env_doc_id'] == document_id]

    # Group by 'visitor_country' and count the occurrences
    views_by_country = data_for_document.groupby(
        "visitor_country").size().rename('count').sort_values(ascending=False)

    # Plot the views by country for the specific document
    views_by_country.plot(kind="bar", figsize=(
        9, 5), title=f"Views by Country for Document {document_id}")
    plt.show()  # This will display the plot
