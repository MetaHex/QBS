redirectToCheckout = function (sessionId) {
    var stripe = Stripe('pk_test_51L7jXIDnRN08I7PBJHEn6Phz703EuhCtxh5RZlmX3HjplZlddVsfmfMGehFMdFiPJLy2uTpbaG59N4tNzRwiTnLj00wIgfXFeg');
    stripe.redirectToCheckout({
        sessionId: sessionId
    });
};