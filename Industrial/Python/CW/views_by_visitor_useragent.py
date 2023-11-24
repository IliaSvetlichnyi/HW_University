import pandas as pd
import matplotlib.pyplot as plt

def views_by_visitor_useragent(data, document_id):
    # Filter data for the specific document_id
    data_for_document = data[data['env_doc_id'] == document_id].copy()
    views_by_visitor_useragent = data_for_document.groupby(
        "visitor_useragent").size().sort_values(ascending=False)

    # Plot the top browsers by views
    views_by_visitor_useragent.plot(
        kind="bar", figsize=(9, 5), title="Views by Browser")
    plt.show()