from datetime import datetime


def time_dif( f_time:str, t_time:str ):
    # Define the two times
    time1 = datetime.strptime(f_time, '%H:%M:%S.%f')
    time2 = datetime.strptime(t_time, '%H:%M:%S.%f')

    # Calculate the time difference
    difference = time2 - time1

    # Extract the difference in seconds
    difference_in_seconds = difference.total_seconds()

    print(f"Difference in seconds: {difference_in_seconds}")

time_dif( "21:09:30.08", "21:14:10.16" )