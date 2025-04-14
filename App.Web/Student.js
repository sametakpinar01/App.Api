// Öğrencileri listeleme fonksiyonu
function GetAllStudents() {
  const studentList = document.getElementById("studentList");

  // API'den öğrenci listesini GET isteği ile al
  fetch("https://localhost:7291/api/Student", { method: "GET" })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Öğrenci verileri alınamadı.");
      }
      return response.json();
    })
    .then((students) => {
      // Öğrenci listesini tabloya ekle
      studentList.innerHTML = "";
      students.forEach((student) => {
        studentList.innerHTML += `<tr>
                    <th scope="row">${student.no}</th>
                    <td>${student.firstName}</td>
                    <td>${student.lastName}</td>
                    <td>${student.class}</td>
                    <td class="btn-group">
                        <button type="button" class="btn btn-warning" onclick="GetOneStudentForUpdate(${student.id})">Güncelle</button>
                        <button type="button" class="btn btn-danger" onclick="DeleteStudent(${student.id})">Sil</button>
                    </td>
                </tr>`;
      });
    })
    .catch((error) => {
      alert("Hata: " + error.message);
    });
}

// Öğrenci kayıt formu submit işlemi
document
  .getElementById("studentForm")
  .addEventListener("submit", function (event) {
    event.preventDefault();

    // Form doğrulama kontrolü
    if (!this.checkValidity()) {
      this.classList.add("was-validated");
      return;
    }

    // Formdan verileri al ve JSON formatında bir obje oluştur
    const formData = new FormData(this);
    const student = {
      no: formData.get("no"),
      firstName: formData.get("firstName"),
      lastName: formData.get("lastName"),
      class: formData.get("class"),
    };

    // API'ye POST isteği ile öğrenci verilerini gönder
    fetch("https://localhost:7291/api/Student", {
      method: "POST",
      headers: {
        "Content-Type": "application/json", // JSON veri gönderiyoruz
      },
      body: JSON.stringify(student), // Öğrenci verisini JSON formatına çevirerek gönderiyoruz
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Öğrenci eklenemedi.");
        }
        return response.json(); // Başarılıysa JSON verisini al
      })
      .then(() => {
        // Başarılı bir şekilde öğrenci eklendiğinde mesaj göster
        alert("Öğrenci başarıyla kaydedildi!");

        // Formu sıfırla ve modal'ı kapat
        document
          .getElementById("studentForm")
          .classList.remove("was-validated");
        bootstrap.Modal.getInstance(
          document.getElementById("registerStudentModal")
        ).hide();
        document.getElementById("studentForm").reset();

        // Öğrencilerin güncellenmiş listesini al ve görüntüle
        GetAllStudents();
      })
      .catch((error) => {
        // Hata durumunda kullanıcıyı uyar
        alert("Hata: " + error.message);
      });
  });

// Güncelleme için bir öğrencinin bilgilerini al
function GetOneStudentForUpdate(id) {
  fetch(`https://localhost:7291/api/Student/${id}`)
    .then((response) => {
      if (!response.ok) {
        throw new Error("Öğrenci bilgileri alınamadı.");
      }
      return response.json();
    })
    .then((student) => {
      // Güncelleme modalındaki alanları doldur
      document.getElementById("updateStudentId").value = student.id;
      document.getElementById("updateStudentNumber").value = student.no;
      document.getElementById("updateFirstName").value = student.firstName;
      document.getElementById("updateLastName").value = student.lastName;
      document.getElementById("updateClass").value = student.class;

      // Güncelleme modalını göster
      new bootstrap.Modal(document.getElementById("updateStudentModal")).show();
    })
    .catch((error) => {
      alert("Hata: " + error.message);
    });
}

// Öğrenci güncelleme formu submit işlemi
document
  .getElementById("updateStudentForm")
  .addEventListener("submit", function (event) {
    event.preventDefault();

    // Form doğrulama kontrolü
    if (!this.checkValidity()) {
      this.classList.add("was-validated");
      return;
    }

    // Güncellenmiş öğrenci bilgilerini al
    const student = {
      id: document.getElementById("updateStudentId").value,
      no: document.getElementById("updateStudentNumber").value,
      firstName: document.getElementById("updateFirstName").value,
      lastName: document.getElementById("updateLastName").value,
      class: document.getElementById("updateClass").value,
    };

    // API'ye PUT isteği ile güncellenmiş verileri gönder
    fetch(`https://localhost:7291/api/Student/${student.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(student),
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Öğrenci güncellenemedi.");
        }
        alert("Öğrenci başarıyla güncellendi!");
        bootstrap.Modal.getInstance(
          document.getElementById("updateStudentModal")
        ).hide();
        GetAllStudents();
      })
      .catch((error) => {
        alert("Hata: " + error.message);
      });
  });

// Öğrenci silme işlemi
let studentToDeleteId = null;

function DeleteStudent(id) {
  studentToDeleteId = id;
  const deleteModal = new bootstrap.Modal(
    document.getElementById("deleteConfirmationModal")
  );
  deleteModal.show();
}

// Silme işlemini onayla
document
  .getElementById("confirmDeleteButton")
  .addEventListener("click", function () {
    if (studentToDeleteId === null) return;

    // API'ye DELETE isteği gönder
    fetch(`https://localhost:7291/api/Student/${studentToDeleteId}`, {
      method: "DELETE",
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Öğrenci silinemedi.");
        }
        alert("Öğrenci başarıyla silindi!");
        bootstrap.Modal.getInstance(
          document.getElementById("deleteConfirmationModal")
        ).hide();
        GetAllStudents();
      })
      .catch((error) => {
        console.error("Hata:", error);
        alert("Hata: " + error.message);
      });
  });

// Sayfa yüklendiğinde öğrencileri listele
window.onload = function () {
  GetAllStudents();
};
