# Giới thiệu

## Đặt vấn đề

Các chủ cửa hàng nhỏ thường phải đối mặt với những thách thức trong việc quản lý hàng tồn kho và hóa đơn một cách hiệu quả và chính xác. Việc họ cần một ứng dụng điện thoại cầm tay để theo dõi lượng hàng tồn, và ghi chép lại doanh số bán hàng, hóa đơn sẽ giúp tiết kiệm thời gian và giảm thiểu tổn thất sai sót do việc thủ công bàn giấy.

## Mục tiêu và phạm vi

Đề tài "**Hệ thống ứng dụng quản lý hàng tồn kho và hóa đơn**" là dự án xây dựng một hệ thống gồm các _cơ sở dữ liệu_, _API_ và tạo _ứng dụng đa nền tảng_ dễ sử dụng để quản lí hàng tồn kho và doanh số bán hàng cho các cửa hàng nhỏ. Dưới đây là các mục tiêu và phạm vi chính của đề tài:

- **Giao diện dễ sử dụng**: Các nhân viên và chủ cửa hàng có thể dễ sử dụng mà không yêu cầu nhiều kinh nghiệm với công nghệ thông tin.
- **Hỗ trợ đa nền tảng**: Đảm bảo ứng dụng có khả năng hoạt động với các nền tảng và thiết bị khác nhau như là di động (Android, iOS), máy tính (Windows).
- **Quản lý tồn kho**: Giúp người dùng theo dõi và thao tác với thông tin, số lượng, giá tiền của các mặt hàng nhanh chóng.
- **Quản lý hóa đơn**: Cho phép người dùng tạo, chỉnh sửa và lưu trữ hóa đơn cho các giao dịch bán hàng, nhập hàng.
- **Quản lý khách hàng**: Hỗ trợ người dùng quản lý thông tin liên hệ, các giao dịch với các khách hàng.
- **Thống kê**: Cung cấp cho người dùng các báo cáo và thống kê về doanh số, tồn kho và chi phí.
- **Lưu trữ đám mây**: Các dữ liệu của cửa hàng có thể truy cập và sử dụng bất kỳ lúc nào, nơi nào tương ứng quyền hạn được giao bởi quản trị viên.
- **Hoàn toàn self-hosting**: Các chủ cửa hàng có thể nắm giữ mọi dữ liệu, kết nối với các ứng dụng khác mà không phụ thuộc vào sự cung cấp dịch vụ của bên thứ ba.

## Khảo sát tương tự

Trên thị trường hiện nay đã có ứng dụng giúp các doanh nghiệp quản lý tồn kho, hóa đơn và khách hàng như là **KiotViet**. Họ cung cấp đầy đủ chức năng nhưng đối tượng khách hàng nhắm đến quá rộng và ứng dụng của họ được thêm các chức năng mà các cửa hàng nhỏ hầu như không sử dụng.

- **Quản lý và bán hàng**: Họ tách ra làm riêng 2 ứng dụng riêng lẻ khiến việc đổi ứng dụng để sử dụng khiến thêm thao tác thừa tốn thời gian.
- **Các chức năng thương mại thừa**: Ứng dụng của họ thường có thêm các nút dẫn tới việc mời gọi trả thêm tiền dịch vụ để sử dụng các chức năng độc quyền của họ như là dịch vụ vận chuyển hàng hóa riêng, sàn thương mại điện tử riêng, dịch vụ cung cấp website bán hàng, vay vốn...
- **Giao diện phức tạp**: Do quảng cáo thêm các dịch vụ của họ nên phần lớn diện tích màn hình bị lãng phí, gây rối mắt.
- **Dịch vụ theo tháng**: Để sử dụng ứng dụng, các cửa hàng phải bỏ ra một khoản tiền hàng tháng để có thể sử dụng và bị giới hạn chức năng, giới hạn số tài khoản theo gói đăng kí.
- **Dữ liệu không hoàn toàn kiểm soát**: Các dữ liệu khách hàng, hàng hóa đều nằm trên hệ thống của họ, khiến việc nếu các chủ cửa hàng muốn hoàn toàn nắm giữ dữ liệu và trích xuất ra các phần mềm khác bị khó khăn, phụ thuộc.
