function initializeBookById(bookId) {
    const book = document.getElementById(bookId);

    if (!book) {
        console.warn("Book not found.");
        return;
    }

    if (book.dataset.initialized === "true") {
        return;
    }

    book.dataset.initialized = "true";

    const prevBtn = document.getElementById(`prev-${bookId}`);
    const nextBtn = document.getElementById(`next-${bookId}`);

    if (!prevBtn || !nextBtn) {
        console.warn("Buttons not found.");
        return;
    }

    const papers = Array.from(book.querySelectorAll(".paper"));
    const numOfPapers = papers.length;

    papers.forEach((paper, index) => {
        paper.style.zIndex = numOfPapers - index;
    });

    let currentLocation = 0;

    prevBtn.addEventListener("click", goPrevPage);
    nextBtn.addEventListener("click", goNextPage);

    function openBook() {
        book.style.transform = "translateX(50%)";
        prevBtn.style.transform = "translateX(-180px)";
        nextBtn.style.transform = "translateX(180px)";
    }

    function closeBook(isAtBeginning) {
        if (isAtBeginning) {
            book.style.transform = "translateX(0%)";
        } else {
            book.style.transform = "translateX(100%)";
        }
        prevBtn.style.transform = "translateX(0px)";
        nextBtn.style.transform = "translateX(0px)";
    }

    function goNextPage() {
        if (currentLocation < numOfPapers) {
            const paper = papers[currentLocation];
            paper.classList.add("flipped");
            paper.style.zIndex = currentLocation;

            currentLocation++;

            if (currentLocation === numOfPapers) {
                closeBook(false);
            } else {
                openBook();
            }
        }
    }

    function goPrevPage() {
        if (currentLocation > 0) {
            currentLocation--;

            const paper = papers[currentLocation];
            paper.classList.remove("flipped");
            paper.style.zIndex = numOfPapers - currentLocation;

            if (currentLocation === 0) {
                closeBook(true);
            } else {
                openBook();
            }
        }
    }
}
