import matplotlib.pyplot as plt
import pandas as pd
import json


def views_by_country(data, document_id):
    data_for_document = data[data['env_doc_id'] == document_id]
    views_by_country = data_for_document.groupby(
        "visitor_country").size().rename('count').sort_values(ascending=False)

    views_by_country.plot(kind="bar", figsize=(
        9, 7), title=f"Views by Country for Document {document_id}")
    plt.show() 
