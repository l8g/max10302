import cv2

cap = cv2.VideoCapture('output_yuy2.avi')
fourcc_code = int(cap.get(cv2.CAP_PROP_FOURCC))
fourcc_str = "".join([chr((fourcc_code >> 8 * i) & 0xFF) for i in range(4)])
print(fourcc_str)
