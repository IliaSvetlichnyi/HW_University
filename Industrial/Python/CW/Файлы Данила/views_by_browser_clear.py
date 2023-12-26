import pandas as pd
import matplotlib.pyplot as plt

def views_by_browser_clear(data, document_id):
    # Filter data for the specific document_id
    data_for_document = data[data['env_doc_id'] == document_id].copy()
    data_for_document['browser_type'] = data_for_document['visitor_useragent'].str.split(
        '/').str[0]
    views_by_browser = data_for_document.groupby(
        "browser_type").size().sort_values(ascending=False)

    # Plot the top browsers by views
    views_by_browser.plot(kind="bar", figsize=(9, 7), title="Views by Browser")
    plt.show()

