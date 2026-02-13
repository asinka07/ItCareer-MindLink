function initializeBook() {
    const prevBtn = document.querySelector("#prev-btn");
    const nextBtn = document.querySelector("#next-btn");
    const book = document.querySelector("#book");

    // взимаме всички страници
    const papers = Array.from(document.querySelectorAll(".book .paper"));
    const numOfPapers = papers.length;

    // задаваме първоначален z-index
    papers.forEach((paper, index) => {
        paper.style.zIndex = numOfPapers - index;
    });

    let currentLocation = 0; // започваме с първата страница не обърната

    // Event listeners
    prevBtn.addEventListener("click", goPrevPage);
    nextBtn.addEventListener("click", goNextPage);

    // Функция за отваряне на книгата (местим центъра и бутоните)
    function openBook() {
        book.style.transform = "translateX(50%)";
        prevBtn.style.transform = "translateX(-180px)";
        nextBtn.style.transform = "translateX(180px)";
    }

    // Функция за затваряне на книгата (начало или край)
    function closeBook(isAtBeginning) {
        if (isAtBeginning) {
            book.style.transform = "translateX(0%)";
        } else {
            book.style.transform = "translateX(100%)";
        }
        prevBtn.style.transform = "translateX(0px)";
        nextBtn.style.transform = "translateX(0px)";
    }

    // Флип на следваща страница
    function goNextPage() {
        if (currentLocation < numOfPapers) {
            const paper = papers[currentLocation];
            paper.classList.add("flipped");

            // след като е обърната, слагаме z-index така че предните страници да са отдолу
            paper.style.zIndex = currentLocation;

            currentLocation++;

            // ако стигнем последната страница, затваряме книгата в дясно
            if (currentLocation === numOfPapers) {
                closeBook(false);
            } else {
                openBook();
            }
        }
    }

    // Флип на предишна страница
    function goPrevPage() {
        if (currentLocation > 0) {
            currentLocation--;

            const paper = papers[currentLocation];
            paper.classList.remove("flipped");

            // възстановяваме z-index, за да се вижда коректно stack-а
            paper.style.zIndex = numOfPapers - currentLocation;

            // ако сме на първата страница, затваряме книгата вляво
            if (currentLocation === 0) {
                closeBook(true);
            } else {
                openBook();
            }
        }
    }
}