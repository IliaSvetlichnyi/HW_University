import pandas as pd
import json
import graphviz
import subprocess
import tkinter as tk
from tkinter import ttk

from graphviz import Digraph


def upload_data(file_path):
    # Initialize an empty list to store JSON objects
    json_data = []

    # Load each JSON object from each line
    with open(file_path, 'r') as file:
        for line in file:
            try:
                json_object = json.loads(line)
                json_data.append(json_object)
            except json.JSONDecodeError:
                print("Error decoding JSON from line:", line)

    # Convert the list of JSON objects to a DataFrame
    df = pd.DataFrame(json_data)
    return df

def get_readers(doc_uuid, df):
    """Returns all visitor UUIDs of readers of the specified document."""
    return df[df["env_doc_id"] == doc_uuid]["visitor_uuid"].unique()


def get_read_docs(visitor_uuid, df):
    """Returns all document UUIDs read by the specified visitor."""
    return df[df["visitor_uuid"] == visitor_uuid]["env_doc_id"].unique()


def sort_by_readers(doc_uuid, df):
    """Sorting function based on the number of readers."""
    root = tk.Tk()
    root.title("Top 10")

    for index, element in enumerate(get_readers(doc_uuid, df)):
        label = tk.Label(root, text=f"Element {index + 1}: {element}")
        label.pack()

    root.mainloop()
    return len(get_readers(doc_uuid, df))




def also_like(doc_uuid, df, visitor_uuid=None, sorting_func=sort_by_readers):
    """Returns a list of top 10 'liked' documents based on the sorting function."""
    readers = get_readers(doc_uuid, df)

    if visitor_uuid:
        readers = [r for r in readers if r != visitor_uuid]

    all_read_docs = [get_read_docs(r, df) for r in readers]
    read_docs = list(
        set([doc for sublist in all_read_docs for doc in sublist if doc != doc_uuid]))

    read_docs_sorted = sorted(
        read_docs, key=lambda x: sorting_func(x, df), reverse=True)

    return read_docs_sorted[:10]


# Function d: Generate graph representation (to be implemented using Graphviz)
def create_graph(doc_uuid, df):
    # Initialize Graphviz Digraph
    dot = Digraph(comment='Also Likes Graph')
    dot.attr(rankdir='TB')  # Top to Bottom layout

    # Create the 'also like' documents subgraph
    with dot.subgraph(name='cluster_also_like') as c:
        c.attr(color='none')
        c.attr(label='Documents')
        related_docs = also_like(doc_uuid, df)
        # Create nodes for related documents
        for doc in related_docs:
            if pd.isna(doc):
                continue
            doc_id = str(doc)[-4:]
            c.node(doc_id, doc_id, shape='box', color='lightgrey')

    # Create a node for the input document
    dot.node(doc_uuid[-4:], doc_uuid[-4:], shape='box', color='green')

    # Create the readers subgraph
    with dot.subgraph(name='cluster_readers') as c:
        c.attr(color='none')
        c.attr(label='Readers')
        readers = get_readers(doc_uuid, df)
    # Create edges between the specified document and its readers
    for reader in readers:
        if pd.isna(reader):
            continue
        reader_id = str(reader)[-4:]
        # Add edge from each reader to the specified document
        dot.edge(reader_id, doc_uuid[-4:], color='green')

    # Create edges between readers and other documents they have read
    for reader in readers:
        if pd.isna(reader):
            continue
        reader_id = str(reader)[-4:]
        read_docs = get_read_docs(reader, df)
        for doc in read_docs:
            if pd.isna(doc) or doc == doc_uuid:
                continue
            doc_id = str(doc)[-4:]
            # black or another color to differentiate
            dot.edge(reader_id, doc_id, color='black')

    # Save the graph
    output_path = '/Users/daniilalekseev/Desktop/cw/also_likes_graph'
    dot.render(output_path, format='pdf', cleanup=True)

    result = output_path + '.pdf'
    subprocess.Popen(['open', result])

    return result




