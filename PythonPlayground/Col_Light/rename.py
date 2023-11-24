import os

def rename_files(directory, old_string, new_string):
    # Iterate over all the files in the specified directory
    for filename in os.listdir(directory):
        # Check if the old_string is in the filename
        if old_string in filename:
            # Construct the new filename
            new_filename = filename.replace(old_string, new_string)
            # Renaming the file
            os.rename(os.path.join(directory, filename), os.path.join(directory, new_filename))
            print(f"Renamed '{filename}' to '{new_filename}'")

# Example Usage:
rename_files('./', '5', 'Blue')

